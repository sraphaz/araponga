// Jest setup for DevPortal tests
// This file runs before each test

// Polyfill para TextEncoder/TextDecoder (necessário para jsdom em Node.js < 18)
const { TextEncoder, TextDecoder } = require('util');
global.TextEncoder = TextEncoder;
global.TextDecoder = TextDecoder;

// Suppress console warnings in tests if needed
global.console = {
  ...console,
  warn: jest.fn(),
  error: jest.fn(),
};
