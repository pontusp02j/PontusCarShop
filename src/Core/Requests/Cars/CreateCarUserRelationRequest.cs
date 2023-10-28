namespace Core.Requests.Cars
{
    public class CreateCarUserRelationRequest
    {
        public int UsersId { get; set; }
        public int ViewedCarsId { get; set; }
    }
}
