using AutoMapper;


namespace Core.Utilities
{
    public class NullableDateTimeToUniversalTimeResolver : IValueResolver<object, object, DateTime?>
    {
        public DateTime? Resolve(object source, object destination, DateTime? sourceMember, ResolutionContext context)
        {
            return sourceMember?.ToUniversalTime();
        }
    }
}
