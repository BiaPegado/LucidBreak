using UnityEngine;

/// <summary>
/// Exemplo prático de como usar o sistema de interação de objetos
/// Este script demonstra diferentes cenários de uso
/// </summary>
public class ExemploInteracao : MonoBehaviour
{
    [Header("Exemplo 1: Interação Simples")]
    [Tooltip("Objeto que será mostrado quando interagir com a chave")]
    public GameObject porta;
    
    [Header("Exemplo 2: Sequência Complexa")]
    [Tooltip("Objeto que será mostrado primeiro")]
    public GameObject luzVermelha;
    [Tooltip("Objeto que será mostrado depois")]
    public GameObject luzVerde;
    [Tooltip("Texto que aparecerá")]
    public GameObject textoAlerta;
    
    [Header("Referências dos Scripts")]
    public ObjectInteractionTrigger triggerSimples;
    public SequentialObjectTrigger triggerSequencial;
    
    void Start()
    {
        // Configurar exemplo 1: Chave que abre porta
        ConfigurarExemploChavePorta();
        
        // Configurar exemplo 2: Botão que ativa sequência
        ConfigurarExemploBotaoSequencia();
    }
    
    /// <summary>
    /// Exemplo 1: Chave que abre uma porta
    /// </summary>
    void ConfigurarExemploChavePorta()
    {
        if (triggerSimples != null && porta != null)
        {
            // Configurar o trigger para mostrar a porta
            triggerSimples.objectToToggle = porta;
            triggerSimples.showObject = true;
            triggerSimples.hideAfterInteraction = false;
            
            // Configurar prompt de interação
            triggerSimples.showInteractionPrompt = true;
            triggerSimples.interactKey = KeyCode.E;
            
            Debug.Log("Exemplo 1 configurado: Chave que abre porta");
        }
    }
    
    /// <summary>
    /// Exemplo 2: Botão que ativa uma sequência de eventos
    /// </summary>
    void ConfigurarExemploBotaoSequencia()
    {
        if (triggerSequencial != null)
        {
            // Limpar ações existentes
            triggerSequencial.ClearActions();
            
            // Adicionar sequência de ações
            if (luzVermelha != null)
            {
                triggerSequencial.AddAction(luzVermelha, ObjectAction.ActionType.Show, 0f);
            }
            
            if (luzVerde != null)
            {
                triggerSequencial.AddAction(luzVerde, ObjectAction.ActionType.Show, 1f);
                triggerSequencial.AddAction(luzVerde, ObjectAction.ActionType.Hide, 3f);
            }
            
            if (textoAlerta != null)
            {
                triggerSequencial.AddAction(textoAlerta, ObjectAction.ActionType.Show, 2f);
                triggerSequencial.AddAction(textoAlerta, ObjectAction.ActionType.Hide, 4f);
            }
            
            Debug.Log("Exemplo 2 configurado: Botão com sequência de eventos");
        }
    }
    
    /// <summary>
    /// Método para testar a interação via código
    /// </summary>
    [ContextMenu("Testar Interação Simples")]
    public void TestarInteracaoSimples()
    {
        if (triggerSimples != null)
        {
            triggerSimples.TriggerInteraction();
            Debug.Log("Interação simples testada!");
        }
    }
    
    /// <summary>
    /// Método para testar a sequência via código
    /// </summary>
    [ContextMenu("Testar Sequência")]
    public void TestarSequencia()
    {
        if (triggerSequencial != null)
        {
            triggerSequencial.TriggerSequence();
            Debug.Log("Sequência testada!");
        }
    }
    
    /// <summary>
    /// Método para resetar todos os triggers
    /// </summary>
    [ContextMenu("Resetar Todos os Triggers")]
    public void ResetarTriggers()
    {
        if (triggerSimples != null)
        {
            triggerSimples.ResetTrigger();
        }
        
        if (triggerSequencial != null)
        {
            triggerSequencial.ResetTrigger();
        }
        
        Debug.Log("Todos os triggers foram resetados!");
    }
    
    /// <summary>
    /// Exemplo de como conectar eventos
    /// </summary>
    void ConectarEventos()
    {
        if (triggerSimples != null)
        {
            // Conectar evento de início de interação
            triggerSimples.OnInteractionStart.AddListener(() => {
                Debug.Log("Interação iniciada!");
                // Aqui você pode adicionar sons, efeitos, etc.
            });
            
            // Conectar evento de fim de interação
            triggerSimples.OnInteractionEnd.AddListener(() => {
                Debug.Log("Interação finalizada!");
                // Aqui você pode adicionar lógica adicional
            });
        }
        
        if (triggerSequencial != null)
        {
            // Conectar evento de início da sequência
            triggerSequencial.OnSequenceStart.AddListener(() => {
                Debug.Log("Sequência iniciada!");
                // Tocar som de alerta, por exemplo
            });
            
            // Conectar evento de fim da sequência
            triggerSequencial.OnSequenceComplete.AddListener(() => {
                Debug.Log("Sequência completa!");
                // Mostrar mensagem de sucesso, por exemplo
            });
        }
    }
    
    /// <summary>
    /// Exemplo de como criar interação condicional
    /// </summary>
    public void VerificarCondicaoParaInteracao()
    {
        // Exemplo: só permitir interação se o jogador tiver uma chave
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null && player.inventory != null)
        {
            // Verificar se o jogador tem um item específico
            // (você precisaria implementar essa lógica baseada no seu sistema de inventário)
            bool temChave = false; // Substitua pela lógica real
            
            if (temChave)
            {
                // Permitir interação
                if (triggerSimples != null)
                {
                    triggerSimples.enabled = true;
                }
            }
            else
            {
                // Bloquear interação
                if (triggerSimples != null)
                {
                    triggerSimples.enabled = false;
                }
                Debug.Log("Você precisa de uma chave para interagir!");
            }
        }
    }
} 