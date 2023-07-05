using OpenAI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationManager : MonoBehaviour
{
    [SerializeField] private Button sendButton;
    [SerializeField] private InputField inputField;
    [SerializeField] private Text conversationText;
    [SerializeField] private Animator animator;

    private OpenAIApi openai = new OpenAIApi();
    private List<ChatMessage> messages = new List<ChatMessage>();

    private void Awake()
    {
        sendButton.onClick.AddListener(OnSendButtonClicked);
    }

    private async void OnSendButtonClicked()
    {
        var message = inputField.text;
        conversationText.text = $"<b>You:</b> {message}{Environment.NewLine}";
        inputField.text = string.Empty;
        
        sendButton.interactable = false;
        
        messages.Add(new ChatMessage()
        {
            Role = "user",
            Content = message,
        });

        var resp = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Messages = messages,
            Model = "gpt-3.5-turbo",
            MaxTokens = 100,
        });
        
        animator.SetTrigger("Talk");
        
        var response = resp.Choices[0].Message.Content;
        conversationText.text += $"<b>AI:</b> {response}{Environment.NewLine}";
        sendButton.interactable = true;
    }
}
