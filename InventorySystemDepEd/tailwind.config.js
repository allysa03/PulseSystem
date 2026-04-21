// tailwind.config.js
const colors = require('tailwindcss/colors');

module.exports = {
    content: [
        './index.html',
        './wwwroot/index.html',
        './components/**/*.razor',
        './components/**/*.html',
        './**/*.razor',
    ],
    theme: {
        extend: {
            colors: {
                // Full default Tailwind colors
                gray: colors.gray,
                red: colors.red,
                yellow: colors.yellow,
                green: colors.green,
                blue: colors.blue,
                indigo: colors.indigo,
                purple: colors.purple,
                pink: colors.pink,
                orange: colors.orange,
                amber: colors.amber,
                lime: colors.lime,
                emerald: colors.emerald,
                teal: colors.teal,
                cyan: colors.cyan,
                sky: colors.sky,
                violet: colors.violet,
                fuchsia: colors.fuchsia,
                rose: colors.rose,
            },
        },
    },
    safelist: [
        {
            pattern:
                /(text|bg|border)-(gray|red|yellow|green|blue|indigo|purple|pink|orange|amber|lime|emerald|teal|cyan|sky|violet|fuchsia|rose)-(50|100|200|300|400|500|600|700|800|900)/,
        },
    ],
    plugins: [],
};