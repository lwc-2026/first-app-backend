namespace Service.Requests
{
    public class CreateUserServiceRequest
    {
        public string Username { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }

        // constructor
        public CreateUserServiceRequest(string username, string? email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }
    }
}