import { defineConfig } from 'vitepress'

export default defineConfig({
  base: '/TscZebra.Plugin/',
  title: "TSC / Zebra plugin",
  description: "A documentation for nuget package",
  themeConfig: {
    nav: [
      { text: 'Home', link: '/' },
      { text: 'Examples', link: '/markdown-examples' }
    ],
    sidebar: [
      {
        text: 'Examples',
        items: [
          { text: 'Markdown Examples', link: '/markdown-examples' },
          { text: 'Runtime API Examples', link: '/api-examples' }
        ]
      }
    ],
    socialLinks: [
      { icon: 'github', link: 'https://github.com/VladStandard/TscZebra.Plugin' }
    ]
  }
})
