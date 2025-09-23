import { globalIgnores } from 'eslint/config';
import { defineConfigWithVueTs, vueTsConfigs } from '@vue/eslint-config-typescript';
import pluginVue from 'eslint-plugin-vue';
import skipFormatting from '@vue/eslint-config-prettier/skip-formatting';
import pluginVueScopedCss from 'eslint-plugin-vue-scoped-css';

// To allow more languages other than `ts` in `.vue` files, uncomment the following lines:
// import { configureVueProject } from '@vue/eslint-config-typescript'
// configureVueProject({ scriptLangs: ['ts', 'tsx'] })
// More info at https://github.com/vuejs/eslint-config-typescript/#advanced-setup

export default [
  ...defineConfigWithVueTs(
    {
      name: 'app/files-to-lint',
      files: ['**/*.{js,jsx,ts,mts,tsx,vue}'],
    },

    globalIgnores([
      '**/node_modules/**',
      '**/dist/**',
      '**/dist-ssr/**',
      '**/coverage/**',
      '**/.vscode/**',
      '**/.idea/**',
      '**/*.min.js',
      '**/public/**',
      '**/build/**',
    ]),

    pluginVue.configs['flat/recommended'],
    vueTsConfigs.recommended,
  ),

  ...pluginVueScopedCss.configs['flat/recommended'],
  skipFormatting,
  {
    name: 'app/custom-rules',
    rules: {
      'no-console': 'warn',
      'no-debugger': 'warn',
      'no-restricted-imports': [
        'error',
        {
          patterns: ['../*'],
        },
      ],
    },
  },
];
