# ❓ FAQ - Perguntas Frequentes sobre Onboarding

**Guia de Respostas para Dúvidas Comuns**

**Versão**: 1.0  
**Data**: 2025-01-20

---

## 📋 Índice Rápido

- [Instalação e Setup](#instalação-e-setup)
- [Primeiros Passos](#primeiros-passos)
- [Contribuição](#contribuição)
- [Processos e Workflows](#processos-e-workflows)
- [Problemas Comuns](#problemas-comuns)
- [Comunicação](#comunicação)

---

## Instalação e Setup

### P: "dotnet não reconhece comando" ou "dotnet: command not found"

**R**: Você precisa instalar o **.NET SDK 8.0** (não apenas Runtime).

**Windows**:
1. Baixe em: https://dotnet.microsoft.com/download/dotnet/8.0
2. Escolha "SDK" (não Runtime)
3. Execute o instalador
4. Reinicie o terminal/PowerShell
5. Verifique: `dotnet --version` (deve mostrar 8.0.x)

**macOS**:
```bash
# Opção 1: Download direto
# Acesse: https://dotnet.microsoft.com/download/dotnet/8.0
# Baixe e instale o SDK

# Opção 2: Via Homebrew
brew install dotnet
```

**Linux (Ubuntu/Debian)**:
```bash
# Adicione o repositório Microsoft
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb

# Instale o SDK
sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0
```

**Verificação**: Após instalar, abra um novo terminal e execute:
```bash
dotnet --version
# Deve mostrar: 8.0.x
```

---

### P: "git não reconhece comando" ou "git: command not found"

**R**: Você precisa instalar o Git.

**Windows**:
1. Baixe em: https://git-scm.com/download/win
2. Execute o instalador (aceite padrões)
3. Reinicie o terminal

**macOS**:
```bash
# Via Homebrew
brew install git

# Ou baixe de: https://git-scm.com/download/mac
```

**Linux (Ubuntu/Debian)**:
```bash
sudo apt update
sudo apt install git
```

**Verificação**:
```bash
git --version
# Deve mostrar algo como: git version 2.40.0 ou superior
```

---

### P: O Cursor não abre ou não funciona

**R**: Verifique:

1. **Sistema Operacional**: Cursor requer Windows 10+, macOS 10.15+, ou Linux moderno
2. **Download Correto**: Baixe de https://cursor.sh/ para seu sistema
3. **Permissões**: No macOS/Linux, pode precisar dar permissão de execução
4. **Antivírus**: Alguns antivírus bloqueiam - adicione exceção se necessário

**Troubleshooting**:
- Feche completamente o Cursor e abra novamente
- Reinicie o computador
- Verifique logs de erro (Cursor geralmente mostra mensagem)

---

### P: "dotnet restore" falha com erro de conexão

**R**: Problemas de conexão ao baixar pacotes NuGet.

**Soluções**:
1. **Verifique internet**: Certifique-se de ter conexão estável
2. **Proxy/Firewall**: Se estiver em rede corporativa, configure proxy
3. **NuGet Sources**: Verifique fontes do NuGet:
   ```bash
   dotnet nuget list source
   ```
4. **Limpe cache**: Tente limpar cache do NuGet:
   ```bash
   dotnet nuget locals all --clear
   dotnet restore
   ```

---

### P: "dotnet build" falha com erros

**R**: Pode ser várias coisas. Verifique:

1. **.NET SDK correto**: Deve ser 8.0.x
   ```bash
   dotnet --version
   ```

2. **Dependências restauradas**: Execute primeiro:
   ```bash
   dotnet restore
   ```

3. **Erros específicos**: Leia a mensagem de erro completa. Erros comuns:
   - **CS0006**: Arquivo de metadados não encontrado - Execute `dotnet restore`
   - **CS0234**: Namespace não existe - Verifique versão do SDK
   - **CS0246**: Tipo não encontrado - Verifique imports

**Se nada funcionar**:
- Verifique se está na pasta correta (`cd Arah`)
- Delete `bin/` e `obj/` e tente novamente:
  ```bash
  rm -rf bin obj  # Linux/macOS
  del /s /q bin obj  # Windows PowerShell
  dotnet clean
  dotnet restore
  dotnet build
  ```

---

## Primeiros Passos

### P: Não sei por onde começar

**R**: Siga este fluxo:

1. **Leia o [Onboarding Público](./ONBOARDING_PUBLICO.md)** - Entenda o projeto
2. **Escolha seu caminho**:
   - Quer construir código? → [Onboarding Desenvolvedores](./ONBOARDING_DEVELOPERS.md)
   - Quer observar e propor? → [Onboarding Analistas](./ONBOARDING_ANALISTAS_FUNCIONAIS.md)
3. **Entre no Discord** (quando configurado) - Sala Pública
4. **Escolha uma tarefa pequena** - Issues marcadas com `good-first-issue`

---

### P: O que é "good-first-issue"?

**R**: Issues marcadas como "good first issue" são **tarefas pequenas e acessíveis** para pessoas que estão começando a contribuir.

**Características**:
- Requerem pouco conhecimento prévio
- São bem documentadas
- Não são complexas tecnicamente
- São ótimas para aprender o projeto

**Como encontrar**:
- No GitHub: Filtre por label `good-first-issue`
- Ou pergunte no Discord

---

### P: Preciso saber tudo antes de começar?

**R**: **Não!** O importante é começar pequeno.

**Você pode**:
- Começar com tarefas simples (documentação, pequenos bugs)
- Aprender enquanto contribui
- Usar o Cursor para te ajudar
- Perguntar quando não souber

**O Cursor te ajuda** a:
- Entender código existente
- Escrever código seguindo padrões
- Aprender fazendo

---

## Contribuição

### P: Como sei se minha proposta é boa?

**R**: Use estes critérios:

1. **Serve ao território?** - Não impõe padrões externos
2. **Fortalece autonomia?** - Não centraliza controle
3. **Foi validada?** - Conversei com 2+ pessoas do território
4. **Está clara?** - Documentei bem a proposta
5. **Alinha com valores?** - Respeita princípios do projeto

**Template**: Use o template de "Proposta Funcional" ao criar Issue no GitHub.

---

### P: Quanto tempo preciso dedicar?

**R**: **Não há requisito mínimo**. Contribua conforme sua disponibilidade.

**Sugestões**:
- **5 minutos/dia**: Leia issues, discuta
- **30 minutos/semana**: Corrija documentação, pequenos bugs
- **2-3 horas/semana**: Implemente funcionalidades pequenas
- **Mais tempo**: Seja mentor, organize coisas

**O importante**: Contribua no seu ritmo, sem pressão.

---

### P: Como faço minha primeira contribuição?

**Para Desenvolvedores**:

1. Escolha uma Issue marcada com `good-first-issue`
2. Comente na Issue: "Vou trabalhar nisso"
3. Crie uma branch: `git checkout -b fix/nome-da-issue`
4. Faça as mudanças (o Cursor ajuda)
5. Teste localmente: `dotnet test`
6. Commit: `git commit -m "fix: descrição da mudança"`
7. Push: `git push origin fix/nome-da-issue`
8. Crie Pull Request no GitHub

**Para Analistas Funcionais**:

1. Observe necessidade no seu território
2. Documente usando template de "Proposta Funcional"
3. Valide com 2-3 pessoas do território
4. Crie Issue no GitHub
5. Discuta no Discord (#propostas-funcionais)

**Para Comunidade**:

1. Use a plataforma (quando disponível)
2. Identifique o que funciona e o que não funciona
3. Reporte via GitHub Issue ou Discord (#feedback-comunidade)
4. Participe de discussões

---

### P: Como crio um Pull Request?

**R**: Passo a passo:

1. **Faça suas mudanças** na sua branch
2. **Commit e push**:
   ```bash
   git add .
   git commit -m "tipo: descrição clara"
   git push origin sua-branch
   ```
3. **No GitHub**: Vá para o repositório, clique em "Pull Requests" > "New Pull Request"
4. **Preencha o template**:
   - Descreva o que mudou e por quê
   - Referencie Issues relacionadas (#número)
   - Marque checklist de documentação
5. **Aguarde review** - Outras pessoas vão revisar

**Tipos de commit** (convenção):
- `feat`: Nova funcionalidade
- `fix`: Correção de bug
- `docs`: Mudança em documentação
- `refactor`: Refatoração de código
- `test`: Adição ou correção de testes

---

### P: Meu PR foi rejeitado. E agora?

**R**: **É normal e faz parte do processo!**

1. **Leia os comentários** - São construtivos, não pessoais
2. **Pergunte** - Se não entender algo, pergunte
3. **Faça ajustes** - Implemente sugestões
4. **Aprenda** - Cada review é oportunidade de aprender

**Lembre-se**:
- Code review não é crítica pessoal
- É sobre código, não sobre você
- Todos passam por isso
- É assim que aprendemos juntos

---

## Processos e Workflows

### P: Como funciona o processo de validação de propostas?

**R**: Veja [`docs/PRIORIZACAO_PROPOSTAS.md`](./PRIORIZACAO_PROPOSTAS.md) para processo completo.

**Resumo rápido**:
1. Você observa necessidade no território
2. Documenta proposta (Issue no GitHub)
3. Valida com comunidade (2+ pessoas)
4. Proposta é discutida no Discord
5. Desenvolvedores avaliam viabilidade técnica
6. Priorização baseada em critérios
7. Implementação (se aprovada)

---

### P: Como decidem o que implementar primeiro?

**R**: Usamos critérios de priorização (veja [`docs/PRIORIZACAO_PROPOSTAS.md`](./PRIORIZACAO_PROPOSTAS.md)).

**Fatores considerados**:
- **Necessidade territorial** - Impacto no território
- **Viabilidade técnica** - É possível implementar?
- **Alinhamento com roadmap** - Está nas 29 fases?
- **Capacidade do time** - Temos pessoas disponíveis?
- **Validação** - Foi validada com comunidades?

**Não decidimos sozinhos** - É processo colaborativo e orgânico.

---

### P: Posso trabalhar em qualquer Issue?

**R**: **Sim, mas:**

1. **Verifique primeiro**:
   - Comente na Issue: "Vou trabalhar nisso"
   - Veja se alguém já está trabalhando
   - Verifique se está clara

2. **Escolha algo adequado**:
   - Se iniciante: `good-first-issue`
   - Se experiente: Qualquer Issue
   - Se analista: Issues marcadas `analista-funcional`

3. **Pergunte se tiver dúvidas**:
   - Discord (#geral ou sala específica)
   - Comentário na Issue
   - GitHub Discussions

---

## Problemas Comuns

### P: Testes falhando localmente mas passando no CI

**R**: Pode ser várias coisas:

1. **Versão do .NET**: Certifique-se de usar 8.0.x
   ```bash
   dotnet --version
   ```

2. **Ambiente diferente**: CI usa Linux, você pode estar em Windows/macOS
   - Alguns testes podem depender de sistema específico
   - Verifique mensagem de erro

3. **Dados não limpos**: Tente limpar e rodar novamente:
   ```bash
   dotnet clean
   dotnet restore
   dotnet test
   ```

4. **Variáveis de ambiente**: CI pode ter variáveis que você não tem
   - Verifique `appsettings.Development.json`
   - Verifique variáveis de ambiente necessárias

**Se persistir**: Reporte na Issue ou Discord, incluindo:
- Mensagem de erro completa
- Versão do .NET
- Sistema operacional
- Output completo do `dotnet test`

---

### P: Erro ao fazer push: "permission denied"

**R**: Você não tem permissão para fazer push direto no repositório principal.

**Solução**:
1. **Faça Fork** do repositório (botão "Fork" no GitHub)
2. **Clone seu fork**:
   ```bash
   git clone https://github.com/SEU-USUARIO/Arah.git
   ```
3. **Adicione remote original** (opcional, para atualizar):
   ```bash
   git remote add upstream https://github.com/sraphaz/Arah.git
   ```
4. **Trabalhe no seu fork**:
   ```bash
   git checkout -b minha-feature
   # Faça mudanças
   git push origin minha-feature
   ```
5. **Crie PR do seu fork** para o repositório principal

---

### P: Como atualizar minha branch com mudanças da main?

**R**: Atualize do upstream:

```bash
# Se você fez fork
git fetch upstream
git checkout main  # ou sua-branch
git merge upstream/main

# Ou se você tem acesso direto
git fetch origin
git checkout main
git pull origin main
```

---

### P: Erro de merge/conflito no PR

**R**: **Conflitos são normais**, especialmente se o PR demorou.

**Como resolver**:

1. **Atualize sua branch**:
   ```bash
   git fetch origin main
   git checkout sua-branch
   git merge origin/main
   ```

2. **Resolva conflitos**:
   - Git mostrará arquivos com conflitos
   - Abra cada arquivo e procure por `<<<<<<<`, `=======`, `>>>>>>>`
   - Escolha o código correto ou combine ambos
   - Remova os marcadores de conflito

3. **Commit a resolução**:
   ```bash
   git add .
   git commit -m "resolve: conflitos com main"
   git push origin sua-branch
   ```

**Dica**: O Cursor pode ajudar a resolver conflitos!

**Se não conseguir**: Peça ajuda no Discord ou na Issue - não se preocupe!

---

## Comunicação

### P: Como peço ajuda?

**R**: Múltiplas formas:

1. **Discord** (mais rápido):
   - Sala Pública: Perguntas gerais
   - #desenvolvedores: Questões técnicas
   - #analistas-funcionais: Questões funcionais
   - #mentoria: Buscar orientação

2. **GitHub**:
   - Comentário na Issue relevante
   - GitHub Discussions para debates
   - Nova Issue se for problema/duvida nova

3. **Não tenha medo de perguntar**:
   - Não existe pergunta "boba"
   - Todos começamos em algum lugar
   - Comunidade é acolhedora

---

### P: Quanto tempo demora para responder Issues/PRs?

**R**: Depende da disponibilidade do time.

**Expectativas realistas**:
- **Issues**: 1-3 dias úteis
- **PRs simples**: 2-5 dias úteis
- **PRs complexos**: Pode levar mais tempo

**Se demorar muito**:
- Mencione no Discord (respeitosamente)
- Adicione comentário na Issue/PR
- Seja paciente - time é orgânico e voluntário

---

### P: Como reporto um bug?

**R**: Use o template de "Bug Report" ao criar Issue no GitHub.

**Inclua**:
- **O que aconteceu**: Descrição clara
- **O que esperava**: Comportamento esperado
- **Como reproduzir**: Passos claros
- **Contexto**: Sistema operacional, versão do .NET, etc.
- **Logs/Erros**: Se houver, cole aqui

**Quanto mais detalhes, mais fácil resolver!**

---

### P: Posso sugerir melhorias mesmo sem saber código?

**R**: **Sim! E é muito bem-vindo!**

**Como**:
1. Observe necessidade no território
2. Documente claramente (use template)
3. Valide com comunidade se possível
4. Crie Issue no GitHub
5. Discuta no Discord

**Sua observação territorial é expertise valiosa!**

---

## Miscelânea

### P: Como posso me tornar mentor?

**R**: **Muito simples!**

1. **Ofereça-se**: Poste no Discord (#geral ou #mentoria):
   ```markdown
   🤝 **Oferecendo Mentoria**
   
   Olá! Posso ajudar iniciantes em [área específica].
   Se alguém precisar de orientação, pode me chamar!
   ```

2. **Seja disponível**: Responda perguntas quando puder
3. **Seja paciente**: Lembre-se de quando você começou
4. **Compartilhe conhecimento**: Isso fortalece toda a comunidade

**Não precisa ser expert** - basta ter vontade de ajudar e alguma experiência.

---

### P: Como marco uma Issue como "good-first-issue"?

**R**: **Apenas mantenedores podem**, mas você pode:

1. **Sugerir**: Comente na Issue: "Isso seria um bom good-first-issue?"
2. **Criar Issues simples**: Se você criar Issue simples, mencione que é para iniciantes
3. **Validar**: Teste se realmente é acessível para iniciantes

**Critérios para good-first-issue**:
- Requer pouco conhecimento prévio
- É bem documentada
- Não é complexa tecnicamente
- Não é urgente

---

### P: O projeto aceita contribuições financeiras?

**R**: **Atualmente não**, mas:

- Você pode contribuir com código, testes, documentação
- Você pode contribuir com observação territorial
- Você pode contribuir com tempo e conhecimento

**Se no futuro houver necessidade financeira**, será comunicado de forma transparente e consciente.

---

### P: Como sei se estou contribuindo corretamente?

**R**: **Sinais de que está no caminho certo**:

✅ Você entendeu a necessidade/Issue  
✅ Sua contribuição alinha com valores do projeto  
✅ Você seguiu padrões de código/documentação  
✅ Você testou suas mudanças  
✅ Você documentou o que fez  
✅ Você está aprendendo e crescendo  

**Se tiver dúvida**: Pergunte! É melhor perguntar do que assumir.

---

### P: Posso trabalhar em mais de uma Issue ao mesmo tempo?

**R**: **Sim, mas seja consciente**:

**Bom**:
- 2-3 Issues pequenas simultaneamente
- Uma Issue por vez se for complexa
- Focar em completar antes de começar outra

**Evite**:
- Muitas Issues abertas sem completar
- Deixar Issues pela metade por muito tempo
- Trabalhar em coisas conflitantes

**Dica**: Complete uma Issue antes de começar outra - é mais satisfatório!

---

### P: Como contribuo se não falo português bem?

**R**: **Não é problema!**

- **Código**: Código é universal
- **Documentação**: Podemos revisar e ajustar português
- **Comunicação**: Inglês também é aceito no Discord/GitHub
- **Valorize sua contribuição**: Não deixe idioma limitar você

**Comunidade é acolhedora** - não se preocupe com perfeição no idioma.

---

## 🆘 Ainda com Dúvidas?

**Entre em contato**:
- **Discord**: Sala Pública ou #geral
- **GitHub**: Crie uma Issue com label `question`
- **GitHub Discussions**: Para debates e perguntas gerais

**Não tenha medo de perguntar** - todos começamos em algum lugar e estamos aqui para ajudar!

---

**Última Atualização**: 2025-01-20  
**Versão**: 1.0

**Dica**: Este FAQ está sempre evoluindo. Se sua pergunta não está aqui, pergunte e podemos adicionar!
