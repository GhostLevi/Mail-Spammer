using System.Threading.Tasks;
using FluentEmail.Core;
using Model;

namespace Services.Interface
{
    public interface IEmailGenerator
    {
        Task<IFluentEmail> GenerateEmail(Person personData);
    }
}