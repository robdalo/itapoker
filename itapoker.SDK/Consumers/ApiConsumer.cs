using itapoker.SDK.Models;
using itapoker.SDK.Requests;
using itapoker.SDK.Responses;
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

    public async Task<AnteUpResponse> AnteUpAsync(AnteUpRequest request)
    {
        return await _restConsumer.PostAsync<AnteUpResponse>(
            endpoint: ApiEndpoints.AnteUp,
            authToken: AuthToken,
            payload: request);
    }

    public async Task<DealResponse> DealAsync(DealRequest request)
    {
        return await _restConsumer.PostAsync<DealResponse>(
            endpoint: ApiEndpoints.Deal,
            authToken: AuthToken,
            payload: request);
    }

    public async Task<List<HighScore>> GetHighScoresAsync()
    {
        return await _restConsumer.GetAsync<List<HighScore>>(
            endpoint: ApiEndpoints.HighScores,
            authToken: AuthToken);
    }

    public async Task<SinglePlayerResponse> SinglePlayerAsync(SinglePlayerRequest request)
    {
        return await _restConsumer.PostAsync<SinglePlayerResponse>(
            endpoint: ApiEndpoints.SinglePlayer, 
            authToken: AuthToken,
            payload: request);
    }
}