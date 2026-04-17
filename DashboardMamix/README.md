# DashboardPekus

Painel corporativo construído em Blazor Server com foco em KPIs, visão gerencial e exportação de relatórios.

## Stack

- .NET 10
- ASP.NET Core Blazor Server
- MudBlazor
- Chart.js
- QuestPDF

## Estrutura

- `Components/`: layout, páginas e componentes de dashboard
- `AppCode/`: regras, modelos e acesso a dados
- `Resources/`: internacionalização
- `wwwroot/`: estilos e scripts

## Status atual

Os dados ainda estão mockados. A estrutura do projeto já separa apresentação, regras de negócio e acesso a dados para futura integração com banco.

## Como rodar

1. Instale o SDK do .NET compatível.
2. Acesse a pasta do projeto principal.
3. Rode `dotnet run`.

## Observação

Esse projeto vale manter como peça de portfólio visual e corporativo, mesmo antes da integração definitiva com banco real.
