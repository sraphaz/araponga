#!/usr/bin/env node

/**
 * Script para garantir que arquivos TypeScript estão em UTF-8 sem BOM
 * Remove BOM se presente e reescreve o arquivo em UTF-8 puro
 */

import fs from 'fs';
import path from 'path';

const filesToFix = [
  'components/content/MermaidDiagram.tsx',
  'components/content/MermaidContent.tsx',
];

function removeBOM(content) {
  // Remove BOM (0xEF 0xBB 0xBF) se presente
  if (content[0] === 0xEF && content[1] === 0xBB && content[2] === 0xBF) {
    return content.slice(3);
  }
  return content;
}

function fixFile(filePath) {
  const fullPath = path.join(process.cwd(), filePath);
  
  if (!fs.existsSync(fullPath)) {
    console.log(`⚠️  Arquivo não encontrado: ${filePath}`);
    return;
  }

  try {
    // Lê como buffer para detectar BOM
    const buffer = fs.readFileSync(fullPath);
    const withoutBOM = removeBOM(buffer);
    
    // Converte para string UTF-8
    const content = withoutBOM.toString('utf8');
    
    // Reescreve em UTF-8 sem BOM
    fs.writeFileSync(fullPath, content, { encoding: 'utf8' });
    
    console.log(`✅ Corrigido: ${filePath}`);
  } catch (error) {
    console.error(`❌ Erro ao corrigir ${filePath}:`, error.message);
    process.exit(1);
  }
}

console.log('🔧 Corrigindo codificação UTF-8 dos arquivos...\n');

filesToFix.forEach(fixFile);

console.log('\n✅ Todos os arquivos foram corrigidos!');
