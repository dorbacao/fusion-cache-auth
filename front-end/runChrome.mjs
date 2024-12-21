import puppeteer from 'puppeteer';

(async () => {
    const browser = await puppeteer.launch({
        headless: false,
        devtools: true
    });
    const page = await browser.newPage();

    // Configurar o throttling da rede para 4G fast
    const client = await page.target().createCDPSession();
    await client.send('Network.emulateNetworkConditions', {
        offline: false,
        downloadThroughput: ((10.0 * 1024 * 1024) / 8), // 2.0 Mbps
        uploadThroughput: ((50 * 1024 ) / 8), // 2.0 Mbps
        latency: 50                                    // 150 ms
    });

    await page.goto('http://localhost:5173/');
})();
