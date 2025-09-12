using itapoker.Core.Domain.Enums;
using itapoker.Core.Domain.Models;

namespace itapoker.Core.Services.Interfaces;

public interface ICardService
{
    HandType GetHandType(List<Card> cards);
}