namespace Framework.Common.Entities.Interfaces
{
    public interface ICurrentUser
    {
        long? Id { get; }

        string UserName { get; }
    }

}
