using Azure.AI.OpenAI;

namespace AIRecruitXcel.Core
{
    public class AnswerEvaluator
    {
        private readonly OpenAIClient _Client = new(Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

        public async Task<string> EvaluateAsync(string jd, string cv, string question, string answer)
        {
            var systemMessage =
                @$"You are an interviewer. You are conducting an interview with the job description: `{jd}`
                   The interviewee's CV is: `{cv}`
                   You've asked the interviewee the following question: `{question}`
                   The interviewee will answer the question and you have to evaluate the answer by following the example below in bullet format

                   #example
                   - On a scale 1-5 how much does the answer fulfill the question?
                   - Does the answer fit the STAR method?
                   - Is the answer relevant to the answer and the job description?
                   - based on the CV, Is the answer relevant to the interviewee's experience?
                   - A summary of the interviewee's answer";

            var userMessage = $"The interviewee's answer is: `{answer}`?";

            var temperature = 1.0f; // vary this between 0.0 and 1.0 to control the randomness of the response. 0.0 is deterministic (less random), 1.0 is more random
            var maxTokens = 200; // vary this to change the maximum number of tokens (which impacts words) to generate in the response

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {

                DeploymentName = "gpt-3.5-turbo",
                Temperature = temperature,
                MaxTokens = maxTokens,
                Messages =
                {
                    // The system message represents instructions or other guidance about how the assistant should behave
                    new ChatRequestSystemMessage(systemMessage),
                    new ChatRequestUserMessage(userMessage),
                }
            };

            var response = await _Client.GetChatCompletionsAsync(chatCompletionsOptions);
            ChatResponseMessage responseMessage = response.Value.Choices[0].Message;
            return responseMessage.Content;
        }
    }
}
