const path = require('path')
const webpack = require('webpack')
const autoprefixer = require('autoprefixer')
const ExtractTextPlugin = require('extract-text-webpack-plugin')

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);

    const extractCss = new ExtractTextPlugin({
        filename: "app.css",
        disable: isDevBuild
    });

    return [{
        module: {
            rules: [
                {
                    test: /\.js$/,
                    loader: 'babel-loader',
                    exclude: /node_modules/,
                    include: path.join(__dirname, 'src'),
                },
                {
                    test: /\.css$/,
                    use: extractCss.extract({
                        use: isDevBuild ? ['css-loader'] : ['css-loader?minimize'],
                        fallback: 'style-loader'
                    })
                },
                // fonts and svg
                { test: /\.woff(\?v=\d+\.\d+\.\d+)?$/, loader: "url?limit=10000&mimetype=application/font-woff" },
                { test: /\.woff2(\?v=\d+\.\d+\.\d+)?$/, loader: "url?limit=10000&mimetype=application/font-woff" },
                { test: /\.ttf(\?v=\d+\.\d+\.\d+)?$/, loader: "url?limit=10000&mimetype=application/octet-stream" },
                { test: /\.eot(\?v=\d+\.\d+\.\d+)?$/, loader: "file" },
                { test: /\.svg(\?v=\d+\.\d+\.\d+)?$/, loader: "url?limit=10000&mimetype=image/svg+xml" },
                {
                    // images
                    test: /\.(ico|jpe?g|png|gif)$/,
                    loader: "file"
                }
            ]
        },
        entry: {
            // The loader will follow all chains of reference from this entry point...
            main: './app/boot.js'
        },
        resolve: {
            alias: {
                'vue$': 'vue/dist/vue.esm.js'
            }
        },
        plugins: [
            new webpack.ProvidePlugin({
                jQuery: 'jquery',
                $: 'jquery',
                jquery: 'jquery',
                'window.jquery': 'jquery',
                'window.jQuery': 'jquery',
                'window.$': 'jquery',
            }),
            new webpack.DefinePlugin({
                'process.env': {
                    'NODE_ENV': JSON.stringify(process.env.NODE_ENV)
                }
            }),
            new webpack.HotModuleReplacementPlugin(),
            new webpack.NoEmitOnErrorsPlugin(),
            extractCss
        ],
        output: {
            path: path.join(__dirname, 'wwwroot', 'dist'),
            publicPath: '/dist/',
            filename: '[name].js'
        }
    }];
}
