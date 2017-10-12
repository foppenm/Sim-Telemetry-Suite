const path = require('path');
const webpack = require('webpack');

module.exports = {
    resolve: {
        // For modules referenced with no filename extension, Webpack will consider these extensions
        extensions: ['.js']
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /(node_modules|bower_components)/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['env']
                    }
                }
            }
        ]
    },
    entry: {
        // The loader will follow all chains of reference from this entry point...
        main: ['./app/boot.js']
    },
    plugins: [
        //new webpack.HotModuleReplacementPlugin()
    ],
    output: {
        path: path.join(__dirname, 'wwwroot', 'dist'),
        publicPath: '/dist/',
        filename: '[name].js'
    },
};
