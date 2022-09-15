import React from 'react';
import * as ReactDom from 'react-dom/client';
import { BrowserRouter } from "react-router-dom";
import { Provider } from "react-redux";
import { App } from './app/app';
import {store} from './store/store';

const root = ReactDom.createRoot(
    document.getElementById('root')
);

root.render(
    <Provider store={store}>
        <BrowserRouter><App apiUrl={'https://localhost:6001'}/></BrowserRouter>
    </Provider>
);