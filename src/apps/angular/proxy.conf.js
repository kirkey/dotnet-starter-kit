const https = require('https');
const fs = require('fs');

// For development with self-signed certificates
const agent = new https.Agent({
  rejectUnauthorized: false
});

module.exports = {
  '/api': {
    target: 'https://localhost:7000',
    secure: false,
    changeOrigin: true,
    pathRewrite: {
      '^/api': '/api'
    },
    agent: agent,
    onProxyReq: (proxyReq) => {
      // Add any headers if needed
    }
  }
};
