import { hljsRazor } from './cshtml-razor.js'

export default {
    iconLinks: [
        {
            icon: 'github',
            href: 'https://github.com/alexnoddings/Tyne',
            title: 'GitHub'
        }
    ],
    configureHljs: function (hljs) {
      hljs.registerLanguage('cshtml-razor', hljsRazor);
    },
}
