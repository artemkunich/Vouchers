const path = require('path');
const HWP = require('html-webpack-plugin');

module.exports = {
  entry: path.join(__dirname, '/src/index.js'),
  output: {
    filename: 'build.js',
    path: path.join(__dirname, '/dist')},
    module:{
        rules:[{
            test: /\.js$/,
            exclude: /node_modules/,
            loader: 'babel-loader'
        },{
            test: /\.(sa|sc|c)ss$/,
            exclude: /\.module\.css$/,
            use: [
                'style-loader', 
                'css-loader',
                'sass-loader'
            ]
        },{
            test: /\.module\.css$/,
            use: [
                'style-loader',
                {
                  loader: 'css-loader',
                  options: {
                    modules: true,
                  },
                },
              ],
            
        }]
    },
    plugins:[
        new HWP({template: path.join(__dirname,'./src/index.html')})
    ]
}