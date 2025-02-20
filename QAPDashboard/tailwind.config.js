module.exports = {
    content: [
        './Pages/**/*.cshtml',
        './Views/**/*.cshtml',
        './Areas/**/*.cshtml',
        './Views/**/**/**/*.cshtml',
    ],
    theme: {
        
        extend: {
            spacing: {
                '18': '4.5rem',
            },
            colors: {
                'bluegw': {
                    50: '#F2F2F2',
                    100: '#E9F3FF',
                    200: '#C0E0FF',
                    300: '#98C8FF',
                    400: '#6FAFFC',
                    500: '#468EF9',  // Current color
                    600: '#3274E4',
                    700: '#235EBD',
                    800: '#0D4689',
                    900: '#06356D',
                },

            },
},
    },
    plugins: [
        require('tailwindcss/plugin')(({ addVariant }) => {
            addVariant('search-cancel', '&::-webkit-search-cancel-button');
        }),
    ],
}
