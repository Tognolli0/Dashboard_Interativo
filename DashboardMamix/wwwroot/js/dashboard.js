/**
 * dashboard.js
 * Gráficos do painel Mamix — Chart.js 4
 * Projeto: DashboardPekus | Pekus Tecnologia
 *
 * Cores idênticas ao screenshot:
 *   Azul    → #6470ef  (vendas/sales)
 *   Verde   → #28c8a0  (receita/revenue)
 *   Roxo    → #c47fff  (lucro/profit)
 */

/* ============================================================
   GRÁFICO DE LINHA — Vendas × Receita × Lucro
   (fiel ao Mamix: linhas suaves com área preenchida em degradê)
   ============================================================ */
window.setupDashboardCharts = (canvasId, labels, dataSales, dataRevenue, dataProfit) => {
    const el = document.getElementById(canvasId);
    if (!el) return;

    // Destrói instância anterior para evitar duplicação em re-renders
    const existing = Chart.getChart(el);
    if (existing) existing.destroy();

    const ctx = el.getContext('2d');

    // Degradês de área — idênticos ao Mamix
    const gradBlue = ctx.createLinearGradient(0, 0, 0, 340);
    gradBlue.addColorStop(0, 'rgba(100, 112, 239, 0.45)');
    gradBlue.addColorStop(1, 'rgba(100, 112, 239, 0.02)');

    const gradGreen = ctx.createLinearGradient(0, 0, 0, 340);
    gradGreen.addColorStop(0, 'rgba(40, 200, 160, 0.30)');
    gradGreen.addColorStop(1, 'rgba(40, 200, 160, 0.02)');

    const gradPurple = ctx.createLinearGradient(0, 0, 0, 340);
    gradPurple.addColorStop(0, 'rgba(196, 127, 255, 0.25)');
    gradPurple.addColorStop(1, 'rgba(196, 127, 255, 0.02)');

    new Chart(ctx, {
        type: 'line',
        data: {
            labels,
            datasets: [
                {
                    label: 'Vendas',
                    data: dataSales,
                    borderColor: '#6470ef',
                    backgroundColor: gradBlue,
                    fill: true,
                    tension: 0.45,        // curva suave
                    pointRadius: 0,
                    pointHoverRadius: 5,
                    pointBackgroundColor: '#6470ef',
                    borderWidth: 2.5
                },
                {
                    label: 'Receita',
                    data: dataRevenue,
                    borderColor: '#28c8a0',
                    backgroundColor: gradGreen,
                    fill: true,
                    tension: 0.45,
                    pointRadius: 0,
                    pointHoverRadius: 5,
                    pointBackgroundColor: '#28c8a0',
                    borderWidth: 2.5
                },
                {
                    label: 'Lucro',
                    data: dataProfit || [],
                    borderColor: '#c47fff',
                    backgroundColor: gradPurple,
                    fill: true,
                    tension: 0.45,
                    pointRadius: 0,
                    pointHoverRadius: 5,
                    pointBackgroundColor: '#c47fff',
                    borderWidth: 2.5
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            interaction: { intersect: false, mode: 'index' },
            plugins: {
                legend: { display: false },   // legenda customizada no HTML
                tooltip: {
                    backgroundColor: '#12263f',
                    titleColor: '#fff',
                    bodyColor: '#adb5c6',
                    padding: 12,
                    cornerRadius: 8,
                    titleFont: { size: 12, weight: '600' },
                    bodyFont: { size: 12 },
                    callbacks: {
                        label: (c) => `  ${c.dataset.label}: ${c.formattedValue}`
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    border: { display: false },
                    grid: { color: '#f1f3f9', drawTicks: false },
                    ticks: { color: '#95aac9', font: { size: 11 }, padding: 8, maxTicksLimit: 6 }
                },
                x: {
                    border: { display: false },
                    grid: { display: false },
                    ticks: { color: '#95aac9', font: { size: 11 } }
                }
            }
        }
    });
};

/* ============================================================
   GRÁFICO RADAR — Top Selling Categories
   (3 datasets coloridos igual ao Mamix)
   ============================================================ */
window.setupRadarChart = (canvasId, labels, eletronicos, roupas, moveis) => {
    const el = document.getElementById(canvasId);
    if (!el) return;

    const existing = Chart.getChart(el);
    if (existing) existing.destroy();

    new Chart(el.getContext('2d'), {
        type: 'radar',
        data: {
            labels,
            datasets: [
                {
                    label: 'Eletrônicos',
                    data: eletronicos,
                    backgroundColor: 'rgba(100, 112, 239, 0.25)',
                    borderColor: '#6470ef',
                    borderWidth: 2,
                    pointBackgroundColor: '#6470ef',
                    pointBorderColor: '#fff',
                    pointBorderWidth: 2,
                    pointRadius: 4
                },
                {
                    label: 'Roupas',
                    data: roupas,
                    backgroundColor: 'rgba(196, 127, 255, 0.20)',
                    borderColor: '#c47fff',
                    borderWidth: 2,
                    pointBackgroundColor: '#c47fff',
                    pointBorderColor: '#fff',
                    pointBorderWidth: 2,
                    pointRadius: 4
                },
                {
                    label: 'Móveis',
                    data: moveis,
                    backgroundColor: 'rgba(40, 200, 160, 0.20)',
                    borderColor: '#28c8a0',
                    borderWidth: 2,
                    pointBackgroundColor: '#28c8a0',
                    pointBorderColor: '#fff',
                    pointBorderWidth: 2,
                    pointRadius: 4
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false },
                tooltip: {
                    backgroundColor: '#12263f',
                    titleColor: '#fff',
                    bodyColor: '#adb5c6',
                    padding: 10,
                    cornerRadius: 8
                }
            },
            scales: {
                r: {
                    beginAtZero: true,
                    max: 100,
                    ticks: { display: false, stepSize: 25 },
                    grid: { color: '#edf2f9' },
                    angleLines: { color: '#edf2f9' },
                    pointLabels: { color: '#5d7186', font: { size: 10, weight: '500' } }
                }
            }
        }
    });
};