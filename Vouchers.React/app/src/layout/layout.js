import React, {useState} from 'react'
import {Sidebar} from './sidebar.js'
import Navbar from './navbar.js'
import { Outlet } from "react-router-dom"
import style from './layout.module.css'

export const Layout = ({api}) => {

    const [sideBarWidth, setSideBarWidth] = useState(250)
    return (
        <>         
            <Sidebar sideBarWidth={sideBarWidth} api={api}/>
            <Navbar sideBarWidth={sideBarWidth} sideBarWidthSetter={setSideBarWidth}/>
            <main className={`${style.mainContainer} bg-light`} style={{paddingLeft:sideBarWidth + 10}}>
                <Outlet/>
            </main>
        </>
    )
} 