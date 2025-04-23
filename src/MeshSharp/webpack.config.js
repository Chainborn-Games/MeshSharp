// webpack.config.js
const path = require('path');

module.exports = {
  entry: './index.ts', // Your entry point
  mode: 'production', // Or 'development' for debugging
  devtool: 'source-map',
  target: 'node',
  output: {
    path: path.resolve(__dirname, 'dist'), // Output directory
    filename: 'mesh-sharp-interop.js', // Output file name
    libraryTarget: 'commonjs2', // Important for NodeApi
    //library: 'MeshSharpInterop', // Set global variable name
    umdNamedDefine: true,
  },
  resolve: {
    extensions: ['.ts', '.js'],
  },
  module: {
    rules: [
      {
        test: /\.ts$/,
        use: 'ts-loader',
        exclude: /node_modules/,
      },
    ],
  },
  optimization: {
    usedExports: true, // Enable tree shaking
    minimize: false, // Enable minification
  },
  experiments: {
    outputModule: false,
  },
  externals: {
    '@meshsdk/core': 'commonjs @meshsdk/core'
  }
};