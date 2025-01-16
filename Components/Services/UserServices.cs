namespace MyPocketTrack.Components.Services;
using System.Text.Json;
using Modals;

public class UserServices
{
    private readonly string _userFilePath;
    private User? _currentUser;

    public UserServices()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appFolder = Path.Combine(appDataPath, "MyPocketTrack");
        if (!Directory.Exists(appFolder)) Directory.CreateDirectory(appFolder);

        _userFilePath = Path.Combine(appFolder, "users.json");
        if (!File.Exists(_userFilePath)) File.WriteAllText(_userFilePath, "[]");
    }

    private async Task<List<User>> GetUsersAsync()
    {
        try
        {
            string jsonString = await File.ReadAllTextAsync(_userFilePath);
            return JsonSerializer.Deserialize<List<User>>(jsonString) ?? new List<User>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading users: {ex.Message}");
            return new List<User>();
        }
    }

    private async Task SaveUsersAsync(List<User> users)
    {
        string jsonString = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_userFilePath, jsonString);
    }

    public async Task<bool> RegisterUserAsync(User user)
    {
        var users = await GetUsersAsync();
        if (users.Any(u => u.Username == user.Username)) return false;

        users.Add(new User { Username = user.Username, Password = user.Password, CurrencyType = user.CurrencyType });
        await SaveUsersAsync(users);
        return true;
    }

    public async Task<User?> ValidateUserAsync(string username, string password)
    {
        var users = await GetUsersAsync();
        return users.FirstOrDefault(u => u.Username == username && u.Password == password);
    }

    public async Task LoginAsync(string username, string password)
    {
        _currentUser = await ValidateUserAsync(username, password);
    }

    public void Logout() => _currentUser = null;

    public bool IsAuthenticated() => _currentUser != null;

    public User? GetCurrentUser() => _currentUser;
}
