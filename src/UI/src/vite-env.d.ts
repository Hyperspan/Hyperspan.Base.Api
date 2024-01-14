/// <reference types="vite/client" />
// vite-env.d.ts

// Declare a custom type for your environment variables
interface ImportMetaEnv {
    APP_API_URL: string;
    APP_NAME: string;
}

// Extend the NodeJS global type if needed
declare namespace NodeJS {
    interface ProcessEnv {
        NODE_ENV: 'development' | 'production' | 'gns';
        // Add other environment variables here if needed
    }
}
