using Microsoft.CognitiveServices.Speech;

var contents = new string[]{
    "文字測試段落一"
    ,"測試段落二"
    ,"三"
};

var path = "target/path";

foreach (var content in contents)
{
    await SynthesisToSpeakerAsync("YourSubscriptionKey", "YourServiceRegion", "YourVoiceName", content, path);
}

//sample code from
//https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/how-to-speech-synthesis?tabs=browserjs%2Cterminal&pivots=programming-language-csharp

//所有語音清單在後下網址可找到，例如: zh-TW-HsiaoChenNeural
//https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support?tabs=tts&WT.mc_id=DOP-MVP-37580#text-to-speech
static async Task SynthesisToSpeakerAsync(string subscriptionKey, string region, string speechSynthesisVoiceName, string text, string path)
{
    var config = SpeechConfig.FromSubscription(subscriptionKey, region);
    config.SpeechSynthesisVoiceName = speechSynthesisVoiceName;

    // Creates a speech synthesizer using the default speaker as audio output.
    using (var synthesizer = new SpeechSynthesizer(config))
    {
        // Receive a text from console input and synthesize it to speaker.
        using (var result = await synthesizer.SpeakTextAsync(text))
        {
            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                //save wav here
                //Console.WriteLine($"Speech synthesized to speaker for text [{text}]");
                System.IO.File.WriteAllBytes(path, result.AudioData);
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                    Console.WriteLine($"CANCELED: Did you update the subscription info?");
                }
            }
        }
    }
}