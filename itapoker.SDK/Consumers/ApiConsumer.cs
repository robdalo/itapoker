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

    public async Task<List<HighScore>> GetHighScoresAsync()
    {
        return await _restConsumer.GetAsync<List<HighScore>>(
            endpoint: ApiEndpoints.GetHighScores,
            authToken: AuthToken);
    }

    public async Task<NewGameResponse> SinglePlayerAsync(NewGameRequest request)
    {
        return await _restConsumer.PostAsync<NewGameResponse>(
            endpoint: ApiEndpoints.NewGame, 
            authToken: AuthToken,
            payload: request);
    }    
}