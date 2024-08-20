namespace Entity
{
    public class UserNotification: BaseEntity<int>, IEntity
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
    }
}
