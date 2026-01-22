/**
 * Script para gerar imagens SVG dos diagramas Mermaid
 * Executa: node scripts/generate-mermaid-diagrams.js
 */

const fs = require('fs');
const path = require('path');

const DEVPORTAL_PATH = path.join(__dirname, '../backend/Araponga.Api/wwwroot/devportal');
const INDEX_HTML = path.join(DEVPORTAL_PATH, 'index.html');
const OUTPUT_DIR = path.join(DEVPORTAL_PATH, 'assets/images/diagrams');

// Garantir que o diretório existe
if (!fs.existsSync(OUTPUT_DIR)) {
  fs.mkdirSync(OUTPUT_DIR, { recursive: true });
}

// Ler o HTML
const htmlContent = fs.readFileSync(INDEX_HTML, 'utf8');

// Extrair todos os diagramas Mermaid
const mermaidRegex = /<div class="mermaid"[^>]*data-diagram="([^"]+)"[^>]*>([\s\S]*?)<\/div>/g;
const diagrams = [];
let match;

while ((match = mermaidRegex.exec(htmlContent)) !== null) {
  const diagramId = match[1];
  const diagramCode = match[2].trim();
  
  // Limpar código: remover entidades HTML e tags XML de forma segura
  // Usar uma função de desescape que processa todas as entidades de uma vez
  function unescapeHtmlEntities(text) {
    // Criar um elemento temporário para usar o parser nativo do navegador/Node
    // Isso evita problemas de double escaping
    const entityMap = {
      '&lt;': '<',
      '&gt;': '>',
      '&quot;': '"',
      '&amp;': '&',
      '&#x27;': "'",
      '&#39;': "'",
      '&#x2F;': '/',
      '&#x60;': '`',
      '&#x3D;': '='
    };
    
    // Processar em ordem reversa para evitar re-substituições
    // Começar com &amp; primeiro para evitar double unescape
    let result = text;
    result = result.replace(/&amp;/g, '&');
    result = result.replace(/&lt;/g, '<');
    result = result.replace(/&gt;/g, '>');
    result = result.replace(/&quot;/g, '"');
    result = result.replace(/&#x27;/g, "'");
    result = result.replace(/&#39;/g, "'");
    result = result.replace(/&#x2F;/g, '/');
    result = result.replace(/&#x60;/g, '`');
    result = result.replace(/&#x3D;/g, '=');
    
    // Remover tags XML/HTML após desescapar
    result = result.replace(/<\/?[a-z][^>]*>/gi, '');
    result = result.replace(/<\/[a-zA-Z][^>]*$/gm, '');
    
    return result;
  }
  
  const cleanedCode = unescapeHtmlEntities(diagramCode).trim();
  
  diagrams.push({
    id: diagramId,
    code: cleanedCode,
    outputPath: path.join(OUTPUT_DIR, `${diagramId}.mmd`)
  });
}

// Salvar arquivos .mmd temporários para usar com mermaid-cli
console.log(`📊 Encontrados ${diagrams.length} diagramas Mermaid:`);
diagrams.forEach(diagram => {
  console.log(`   - ${diagram.id}`);
  fs.writeFileSync(diagram.outputPath, diagram.code, 'utf8');
});

console.log(`\n✅ Arquivos .mmd gerados em: ${OUTPUT_DIR}`);
console.log(`\n📝 Próximos passos:`);
console.log(`   1. Instalar mermaid-cli: npm install -g @mermaid-js/mermaid-cli`);
console.log(`   2. Executar: mmdc -i "${OUTPUT_DIR}/*.mmd" -o "${OUTPUT_DIR}" -e svg`);
console.log(`   3. Ou usar um script automatizado para gerar todos os SVGs`);