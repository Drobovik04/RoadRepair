import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';


// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    base: '/',
    server: {
        port: 56398,
        proxy: {
            '/api': {
                target: 'https://localhost:7055',
                secure: false,      
                configure: (proxy) => {
                        proxy.on('proxyReq', (proxyReq, req) => {
                        proxyReq.setTimeout(120_000); // 2 мин
                    });
                },
            },
            '/uploads': {
                target: 'https://localhost:7055',
                secure: false,
            }
        }
    }
})
