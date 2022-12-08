import * as React from 'react'

interface HeaderProps {
    children: React.ReactNode,
}

export const Header = ({children} : HeaderProps) => { 
    return <h1 className="header mt-2 mb-3">
        {children}
    </h1>
}