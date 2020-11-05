# Programa para localizar string em um banco de dados Oracle
## String de conexão
no arquivo program.cs, linha 17, substitua **String de conexão** pela string de conexão da base Oracle

## Parametros de pesquisa
### chave de pesquisa
no arquivo program.cs linha 16, ajuste a variavel **chave** com o conteudo que deseja localizar, como usa o operadora 'LIKE' vc pode usar '%' como conteúdo

### Tabelas por quantidade de Linhas
nas linhas 25 e 26, delimita a pesquisa nas tabelas que tenham um limite de quantidade de linhas

# Instalação e execução
- Primeiro instalar o SDK o dotnet core 3.1
- Copiar os arquivos para um diretório local
- na linha de comando, ir para o diretório
- executar "dotnet run"
