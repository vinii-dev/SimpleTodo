using SimpleTodo.Domain.Entities;
using TestCommon.Consts;

namespace TestCommon.Factory;

public static class UserFactory
{
    public static User CreateUser(
        string username = UserConsts.Username,
        string hashedPassword = UserConsts.HashedPassword)
    {
        return new(
            username: username,
            hashedPassword: hashedPassword
        );
    }
}
