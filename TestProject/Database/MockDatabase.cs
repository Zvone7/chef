namespace Work.Database
{
    public class MockDatabase
    {
        public Dictionary<Guid, UserDto> Users { get; private set; }

        public MockDatabase(Dictionary<Guid, UserDto> users)
        {
            Users = users;
        }

        public MockDatabase(int seed)
        {
            Users = new Dictionary<Guid, UserDto>();
            for (int i = 0; i < seed; i++)
            {
                var user = new UserDto
                {
                    Id = Guid.NewGuid(),
                    UserName = $"User {i}",
                    Birthday = DateTime.Now.AddYears(-i)
                };
                Users.Add(user.Id, user);
            }
        }
    }
}
