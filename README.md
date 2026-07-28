# Vortex 360° – Navegador Panorâmico Interativo

[![Unity Version](https://img.shields.io/badge/Unity-2022.3.62f3-blue)](https://unity.com/)
[![WebGL](https://img.shields.io/badge/Platform-WebGL-green)](https://itch.io/)
[![License](https://img.shields.io/badge/License-MIT-yellow)](LICENSE)

## Sobre o Projeto

Este projeto foi desenvolvido como parte do processo seletivo para estágio em jogos do **Laboratório Vortex – UNIFOR**. O desafio consistia em criar um navegador panorâmico 360° no estilo **Google Street View**, utilizando o motor Unity e com build para WebGL.

O jogo apresenta:
- 15 imagens reais de uma avenida, conectadas em sequência.
- Navegação por teclado (W/S) e clique do mouse (setas 3D).
- Zoom e rotação da câmera.
- Minimapa dinâmico com rotação estilo GTA.
- Gamificação com coleta de gnomos.
- Feedback sonoro e visual.
- Menu, pausa e créditos.
- Transição suave com DOTween.

---

## 🧱 Arquitetura do Projeto

A arquitetura foi projetada para ser modular, desacoplada e de fácil manutenção. Os principais componentes são:

### 1. Dados – `PanoramaDataSO`
- ScriptableObject que representa cada ponto de interesse.
- Armazena:
  - `panoramaTexture`: a imagem 360°.
  - `minimapPosition`: coordenadas (0 a 1) para posicionamento no minimapa.
  - `neighbors`: lista de outros nós conectados (grafo).

### 2. Gerenciador Principal – `PanoramaManager`
- Controla o estado atual da navegação.
- Mantém o nó atual (`_currentNode`) e troca a textura da esfera.
- Dispara o evento `OnLocationChanged` para atualizar a UI e os marcadores 3D.
- Gerencia a navegação por teclado (W/S) e por clique nas setas.

### 3. Minimapa – `MinimapControllerGTA`
- Desenha todos os nós como pontos no canto da tela.
- Calcula a escala e o offset para centralizar o mapa.
- Rotaciona o container com base na orientação da câmera (`LateUpdate`).
- Possui método público `SetNodeColor()` para destacar nós (ex: gnomo).

### 4. Navegação 3D – `WorldMarkerManager` e `WorldMarkerClick`
- `WorldMarkerManager`: ativa/desativa as setas 3D (frente/trás) com base na posição no grafo.
- `WorldMarkerClick`: script anexado a cada seta; ao clicar, chama o `PanoramaManager` com a direção correspondente.
- As setas são objetos 3D posicionados dentro da esfera e sempre virados para a câmera (Billboard).

### 5. Gamificação – `GnomeManager`
- Sorteia um nó aleatório (diferente do atual).
- Pinta o nó no minimapa de amarelo.
- Instancia o gnomo desativado; ativa somente quando o jogador chega ao nó alvo.
- Ao ser clicado, o gnomo encolhe com DOTween, toca um som, e o contador é atualizado.

### 6. Transição – `StreetViewTransition`
- Anima a câmera para frente na direção do olhar durante a troca de nó.
- Utiliza DOTween para movimento e FOV pulse.
- Inclui limite de raio para evitar saída da esfera.

### 7. UI e Feedback
- `UIMessageManager`: exibe mensagens temporárias com animação de escala.
- `GnomeUICounter`: contador de gnomos coletados.
- `AudioManager`: gerencia sons (clique, coleta, spawn) e música ambiente.
- `PauseManager`: pausa o jogo (ESC) e exibe painel com opções.

---

## 📓 Diário de Bordo – Uso de IA

### Ferramentas Utilizadas
- **DeepSeek** – assistente de programação
- **DOTween** – animações
- **TextMeshPro** – UI avançada
- **Git** – controle de versão
- **Street View Download 360** – captura de imagens

### Prompt de Contexto (Persona)

Antes de começar a implementação, defini um prompt de "setup" para alinhar o DeepSeek com meu estilo de trabalho e as convenções do projeto, para que as respostas seguintes já viessem no formato e padrão que eu precisava, sem retrabalho de adaptação:

> *"Vamos trabalhar juntos no Vortex 360°, um navegador panorâmico interativo em Unity (2022.3.62f3). Siga as convenções que já uso: camelCase/PascalCase, nomes em inglês, [SerializeField] em vez de campos públicos, arquitetura modular com Singletons, ScriptableObjects para dados e eventos C# para comunicação entre sistemas (baixo acoplamento), e DOTween para animações/transições. O projeto replica o funcionamento do Google Street View: navegação entre 15 fotos 360º conectadas em grafo, por teclado e clique em setas 3D, com minimapa dinâmico estilo GTA e uma camada de gamificação (coleta de itens). Vamos implementar sistema por sistema — a cada etapa, gere o código já dentro desses padrões. Confirma que entendeu o escopo e a arquitetura antes de começarmos."*

Esse prompt inicial foi importante porque as sugestões de código do DeepSeek já vieram consistentes com a arquitetura do projeto (Singleton + ScriptableObject + eventos) em todas as conversas seguintes, sem eu precisar reexplicar o padrão a cada novo script.

### Prompts Importantes (implementação)
- *"Crie um sistema de navegação por vizinhos usando ScriptableObjects e eventos no Unity."*
- *"Como implementar um minimapa que gira com a câmera, estilo GTA, com máscara para evitar vazamento?"*
- *"Como integrar DOTween para animar transições e feedbacks visuais sem perder performance?"*

### Dificuldades Encontradas
1. **Shader da esfera** (a partir do prompt de navegação/textura): a IA sugeriu configurações erradas para o `Cull Front`; precisei criar um shader personalizado.
2. **Minimapa** (a partir do prompt do minimapa estilo GTA): o cálculo de escala estava vazando os pontos para fora do container; ajustei manualmente com `Mask` e `mapScale`.
3. **Clique nas setas 3D:** inicialmente usei Raycast, mas era impreciso; substituí por `OnMouseDown` com Colliders.
4. **Transição com movimento** (a partir do prompt do DOTween): a câmera saía da esfera; limitei o `forwardDistance` e adicionei verificação de raio.

### Como Validei as Respostas da IA
- Testei cada script separadamente em cenas limpas.
- Comparei com a documentação oficial da Unity.
- Ajustei parâmetros manualmente até o comportamento desejado.

### Reflexão Crítica
A IA foi extremamente útil para gerar estruturas iniciais e resolver problemas específicos, especialmente depois de alinhado o contexto e as convenções do projeto logo no início — isso reduziu bastante o retrabalho de adaptar código genérico ao meu padrão. Ainda assim, ela errou em detalhes técnicos que exigem conhecimento do motor (como o shader da esfera). O desenvolvedor precisa ter **curadoria** e **entendimento do código** para não copiar cegamente. No fim, o projeto é meu, não da IA.

---

## ✅ Requisitos Bônus Atendidos

| Diferencial | Como foi implementado |
|---|---|
| **A. Mecânicas de gamificação** | Sistema de coleta de gnomos (`GnomeManager`): sorteio de nó alvo, destaque no minimapa, animação de coleta e contador na UI. |
| **B. Feedbacks sonoros e visuais** | `AudioManager` (cliques, coleta, spawn, música ambiente) + `UIMessageManager` (mensagens com animação de escala) + transições e FOV pulse via DOTween. |
| **C. Estilização da página web** | Menu, tela de pausa e créditos com identidade visual própria (ver seção de tecnologias). |

---

## 🎮 Como Jogar

| Ação | Controle |
|------|----------|
| Navegar para frente | `W` ou Seta para Cima |
| Navegar para trás | `S` ou Seta para Baixo |
| Girar a câmera | Arrastar o mouse (botão esquerdo) |
| Zoom | Scroll do mouse |
| Pausar/Despausar | `ESC` |
| Coletar gnomo | Clique com o mouse no gnomo 3D |

---

## 🔗 Links

- **Jogo no Itch.io:** [https://cleytonram.itch.io/vortex-teste-tecnico]

---

## 🛠️ Tecnologias Utilizadas

- Unity 2022.3.62f3 (Built-in Render Pipeline)
- C# .NET Standard 2.1
- DOTween (animação)
- TextMeshPro (UI)
- Git (controle de versão)

---

## 📝 Licença

Este projeto é de uso exclusivo para fins de avaliação no processo seletivo do Laboratório Vortex – UNIFOR.

---

*Desenvolvido por Cleyton Ramsay – 2025* ⚠️ *preencher antes de entregar*