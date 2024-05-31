import { defineConfig } from 'vitepress'

export default defineConfig({
  base: '/TscZebra.Plugin/',
  title: "TSC / Zebra plugin",
  description: "A documentation for nuget package",
  themeConfig: {
    nav: [
      { text: 'Home', link: '/' },
      { text: 'Guide', link: '/guide/' }
    ],
    sidebar: [
      {
        text: 'About',
        items: [
          { text: 'About', link: '/guide/about.md' },
        ]
      },
      {
        text: 'Documentation',
        items: [
          { text: 'Installation', link: '/guide/index.md' },
          { text: 'How to use', link: '/guide/usage.md' }
        ]
      },
      {
        text: 'Examples',
        items: [
          { text: 'Blazor server', link: '/guide/blazor-server-example.md' },
        ]
      }
    ],
    socialLinks: [
      { icon: 'github', link: 'https://github.com/VladStandard/TscZebra.Plugin' }
    ]
  }
})
