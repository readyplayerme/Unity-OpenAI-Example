using OpenAI;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConversationManager : MonoBehaviour
{
    [SerializeField] private Button sendButton;
    [SerializeField] private InputField inputField;
    [SerializeField] private Text conversationText;
    [SerializeField] private Animator animator;

    // TODO: Provide your own API key as a string parameter
    private OpenAIApi openai = new OpenAIApi();
    private List<ChatMessage> messages = new List<ChatMessage>();

    private void Awake()
    {
        sendButton.onClick.AddListener(OnSendButtonClicked);
        
        // Add initial promt message to conversation
        messages.Add(new ChatMessage()
        {
            Role = "system",
            Content = "You are a Ready Player Me avatar, who exists in the metaverse." +
                      "Ready Player Me is the worlds leading avatar platform for games and virtual worlds." +
                      "Thousands of developers, brands, and creators are using Ready Player Me to deliver amazing virtual experiences to millions of users" +
                      "Reply with maximum 32 words.",
        });
    }

    private async void OnSendButtonClicked()
    {
        // Append user message to conversation
        var message = inputField.text;
        conversationText.text = $"<b>You:</b> {message}{Environment.NewLine}";
        inputField.text = string.Empty;
        
        sendButton.interactable = false;
        
        messages.Add(new ChatMessage()
        {
            Role = "user",
            Content = message,
        });

        // Send conversation to OpenAI ChatGPT endpoint
        var resp = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Messages = messages,
            Model = "gpt-3.5-turbo",
            MaxTokens = 32,
        });
        
        var reply = resp.Choices[0].Message.Content;
        
        // Append AI reply to conversation
        messages.Add(new ChatMessage()
        {
            Role = "assistant",
            Content = reply,
        });
        
        // Play talk animation
        animator.SetTrigger("Talk");
        
        conversationText.text += $"<b>AI:</b> {reply}{Environment.NewLine}";
        sendButton.interactable = true;
    }
}
