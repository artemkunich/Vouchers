import * as React from 'react';
import * as ReactDom from 'react-dom/client';
import { BrowserRouter } from "react-router-dom";
import { Provider } from "react-redux";
import { App } from './app/app';
import { store } from './store/store';

const rootElement = document.getElementById('root')

if(rootElement){
    const root = ReactDom.createRoot(
        rootElement
    );
    
    root.render(
        <Provider store={store}>
            <BrowserRouter><App/></BrowserRouter>
        </Provider>
    );
}
