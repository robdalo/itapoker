using itapoker.SDK.Models;
using itapoker.SDK.Requests;
using itapoker.Shared.Config;
using itapoker.Shared.Consumers;

namespace itapoker.SDK.Consumers;

public class ApiConsumer
{
    private RestConsumer _restConsumer;

    private const string AuthToken = "";

    public ApiConsumer(string baseUrl)
    {
        _restConsumer = new RestConsumer(baseUrl);
    }

    public async Task<Game> AnteUpAsync(AnteUpRequest request)
    {
        return await _restConsumer.PostAsync<Game>(
            endpoint: ApiEndpoints.AnteUp,
            authToken: AuthToken,
            payload: request);
    }

    public async Task<Game> BetAsync(AnteUpRequest request)
    {
        return await _restConsumer.PostAsync<Game>(
            endpoint: ApiEndpoints.Bet,
            authToken: AuthToken,
            payload: request);
    }

    public async Task<Game> DealAsync(DealRequest request)
    {
        return await _restConsumer.PostAsync<Game>(
            endpoint: ApiEndpoints.Deal,
            authToken: AuthToken,
            payload: request);
    }

    public async Task<List<HighScore>> HighScoresAsync()
    {
        return await _restConsumer.GetAsync<List<HighScore>>(
            endpoint: ApiEndpoints.HighScores,
            authToken: AuthToken);
    }
    
    public async Task<Game> ShowdownAsync(ShowdownRequest request)
    {
        return await _restConsumer.PostAsync<Game>(
            endpoint: ApiEndpoints.Showdown, 
            authToken: AuthToken,
            payload: request);
    }

    public async Task<Game> SinglePlayerAsync(SinglePlayerRequest request)
    {
        return await _restConsumer.PostAsync<Game>(
            endpoint: ApiEndpoints.SinglePlayer,
            authToken: AuthToken,
            payload: request);
    }
}