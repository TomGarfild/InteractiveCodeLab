using System.Linq.Expressions;
using InteractiveCodeLab.Infrastructure.Models;
using MongoDB.Driver;

namespace InteractiveCodeLab.Infrastructure.Repositories.Specifications;

public record UserByEmailSpecification(string Email) : ISpecification<UserData>
{
    public Expression<Func<UserData, bool>> Build()
    {
        return user => user.Email == Email;
    }
}