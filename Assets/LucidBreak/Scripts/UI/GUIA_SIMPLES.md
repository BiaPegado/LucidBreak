# Guia Simples - Sistema de Diálogos

## 🚀 Configuração Rápida (5 minutos)

### 1. Baixar a Fonte Determination
- Vá para: https://www.fonts4free.net/determination-font.html
- Baixe os arquivos `.otf` (DTM-Mono.otf e DTM-Sans.otf)
- Importe para a pasta `Assets/LucidBreak/Fonts/` no Unity

### 2. Criar TMP Font Asset
- **Selecione** o arquivo `.otf` no Project Window
- Clique com botão direito → "Create > TextMeshPro > Font Asset"
- Renomeie para "DeterminationFont"

### 3. Configurar o Sistema
- Crie um GameObject vazio na cena
- Adicione o componente `DialogueSetup`
- Arraste a fonte Determination para o campo `determinationFont`
- Clique com botão direito no componente e selecione "Setup Dialogue System"

### 4. Testar
- Clique com botão direito no `DialogueSetup` e selecione "Test Dialogue System"
- Pressione **Enter** ou **Space** para continuar
- Pressione **Escape** para pular

## 📝 Como Criar Diálogos

### Método 1: ScriptableObject (Recomendado)
1. Clique com botão direito no Project
2. Selecione `Create > LucidBreak > Dialogue Data`
3. Configure no Inspector:
   - **Dialogue Lines**: Adicione quantas linhas quiser
   - **Dialogue Text**: Escreva o texto
   - **Text Speed**: Velocidade da digitação (0.05 = normal)
   - **Use Custom Font**: Marque para usar fonte Determination

### Método 2: Código Simples
```csharp
// Diálogo de uma linha
DialogueData dialogue = DialogueBuilder.CreateSimpleDialogue("Olá, mundo!");
DialogueManager.Instance.StartDialogue(dialogue);

// Diálogo de múltiplas linhas
string[] lines = {"Linha 1", "Linha 2", "Linha 3"};
DialogueData dialogue = DialogueBuilder.CreateDialogueFromLines(lines);
DialogueManager.Instance.StartDialogue(dialogue);
```

### Método 3: Arquivo de Texto
1. Crie um arquivo `.txt` com uma linha por diálogo
2. Use o código:
```csharp
DialogueBuilder builder = GetComponent<DialogueBuilder>();
builder.BuildFromTextFile(textAsset);
builder.BuildAndStartDialogue();
```

## 🎮 Como Colocar Diálogos no Jogo (APENAS POR INTERAÇÃO)

### Opção 1: ItemDialogueTrigger (Recomendado)
1. Adicione `ItemDialogueTrigger` ao GameObject do item
2. Arraste o `DialogueData` para o campo `dialogueData`
3. Configure:
   - `interactionRange`: Distância de interação (2 = normal)
   - `interactKey`: Tecla de interação (E = padrão)
   - `showInteractionPrompt`: true (mostra "Pressione E")

### Opção 2: Código Manual
```csharp
// Em qualquer script
public DialogueData meuDialogue;

void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
    {
        DialogueManager.Instance.StartDialogue(meuDialogue);
    }
}
```

### Opção 3: Botão/Interação
```csharp
public void OnButtonClick()
{
    DialogueData dialogue = DialogueBuilder.CreateSimpleDialogue("Texto do botão!");
    DialogueManager.Instance.StartDialogue(dialogue);
}
```

## 🎨 Personalização

### Cores
- **Background**: Cor de fundo da caixa (preto transparente)
- **Border**: Cor da borda (branco)
- **Text Color**: Cor do texto (por linha)

### Efeitos
- **Shake Text**: Faz o texto tremer
- **Text Speed**: Velocidade da digitação
- **Auto Advance**: Avança automaticamente

### Configurações Globais
- **Can Skip**: Permite pular diálogo
- **Auto Advance**: Avança sozinho
- **Auto Advance Delay**: Tempo de espera

## 📋 Exemplos Práticos

### Diálogo de Item Interativo
```csharp
// No ItemDialogueTrigger
DialogueData itemDialogue = DialogueBuilder.CreateSimpleDialogue(
    "Você encontrou um item misterioso!\nParece ser muito importante..."
);
```

### Diálogo com Efeitos
```csharp
DialogueBuilder builder = GetComponent<DialogueBuilder>();
builder.AddDialogueLine("ALGO DRAMÁTICO ACONTECEU!", 0.08f, true, Color.red);
builder.AddDialogueLine("O mundo nunca mais será o mesmo...", 0.05f, false, Color.white);
builder.BuildAndStartDialogue();
```

### Diálogo de Tutorial
```csharp
string[] tutorialLines = {
    "Use WASD para mover",
    "Pressione E para interagir",
    "Pressione Space para pular",
    "Boa sorte!"
};
DialogueData tutorial = DialogueBuilder.CreateDialogueFromLines(tutorialLines);
DialogueManager.Instance.StartDialogue(tutorial);
```

## 🔧 Controles

- **Enter/Space**: Continuar diálogo
- **Escape**: Pular diálogo (se permitido)
- **E**: Interagir com itens (quando configurado)

## 🎯 Como Usar com Itens

### 1. Criar o Item
1. Crie um GameObject para o item
2. Adicione um Collider2D (Is Trigger = true)
3. Adicione o componente `ItemDialogueTrigger`

### 2. Configurar o Diálogo
1. Crie um `DialogueData` com o texto do item
2. Arraste para o campo `dialogueData` no `ItemDialogueTrigger`

### 3. Testar
1. Certifique-se de que o jogador tem a tag "Player"
2. Aproxime-se do item
3. Pressione E para interagir
4. Use Enter/Space para continuar o diálogo

## ❗ Problemas Comuns

### Diálogo não aparece
- Verifique se o `DialogueManager` está na cena
- Confirme se a fonte Determination está configurada
- Teste com "Test Dialogue System"

### Interação não funciona
- Verifique se o item tem `ItemDialogueTrigger`
- Confirme se o jogador tem a tag "Player"
- Verifique se o Collider2D está como Trigger

### Fonte não aparece
- Verifique se o TMP Font Asset foi criado
- Confirme se está atribuído no `DialogueManager`
- Teste com `useCustomFont = true`

### Múltiplos DialoguePanels ou Cores Inadequadas
**Problema**: Sistema cria múltiplos DialoguePanels com fundo branco e texto invisível

**Solução**:
1. **Adicione o componente `DialogueCleanup`** a qualquer GameObject na cena
2. **Clique com botão direito** no componente
3. **Selecione "Reset Dialogue System"** para limpar duplicatas e corrigir cores
4. **Ou use individualmente**:
   - "Cleanup Duplicate Dialogue Panels" - Remove duplicatas
   - "Fix Dialogue Panel Colors" - Corrige cores

**Prevenção**:
- **Desmarque "Auto Setup"** no DialogueSetup para evitar criação automática
- **Use apenas "Setup Dialogue System"** manualmente quando necessário

## 🎯 Dicas

1. **Use ItemDialogueTrigger** para itens interativos
2. **Mantenha textos curtos** para melhor experiência
3. **Teste sempre** antes de implementar
4. **Use cores diferentes** para destaque
5. **Configure velocidades** adequadas para cada texto
6. **O diálogo só aparece por interação** - não há diálogos automáticos