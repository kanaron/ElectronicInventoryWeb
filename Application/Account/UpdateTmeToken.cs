using Domain.Dto;
using Infrastructure.TmeTokenEncryptionService;
using MediatR;
using Persistence;

namespace Application.Account;

public class UpdateTmeToken
{
    public class Command : IRequest
    {
        public UserDto UserDto { get; set; }
        public string Id { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly AppDbContext _appDbContext;
        private readonly ITmeTokenEncryptionService _encryptionService;

        public Handler(AppDbContext context, ITmeTokenEncryptionService encryptionService)
        {
            _appDbContext = context;
            _encryptionService = encryptionService;
        }


        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var userToEdit = await _appDbContext.Users.FindAsync([request.Id], cancellationToken);

            if (userToEdit == null)
            {
                return;
            }

            userToEdit.TmeToken = _encryptionService.Encrypt(request.UserDto.TmeToken);

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
