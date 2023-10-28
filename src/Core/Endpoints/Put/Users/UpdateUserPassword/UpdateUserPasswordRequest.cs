namespace Core.Endpoints.Put.Users.UpdateUserPasswordEndpoint
{
    public class UpdateUserPasswordRequest
    {
        public int Id { get; set; }
        public string Password { get; set; } = "";
    }
}
