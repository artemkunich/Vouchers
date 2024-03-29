import * as React from 'react'
import style from "./list.module.css"
import "bootstrap-icons/font/bootstrap-icons.css"

interface ListElementProps {
    children: React.ReactNode | React.ReactNode[],
}

interface ListHeaderProps {
    children: React.ReactNode[],
}

interface CollapsedListElementProps {
    children: React.ReactNode[],
}

export const ListHeader = ({children}: ListHeaderProps) => {

    const [isCollapsed, setIsCollapsed] = React.useState(true)
    
    const headStyle = {
        backgroundColor : "#fd7324"
    }

    const bodyStyle = {
        height : isCollapsed ? 0 : 'auto',
        paddingTop : isCollapsed ? 0 : 5,
    }

    return <div className={`container-fluid ${style.listElement}`}>
        <div className={`row ${style.listElementHead}`} style={headStyle} onClick={() => setIsCollapsed(!isCollapsed)}>
            <div className='col-11'>{children[0]}</div>
            <div className='col-1' style={{textAlign: 'right'}}><p className="fs-5">{isCollapsed ? <i className="bi bi-chevron-up"></i> : <i className="bi bi-chevron-down"></i>}</p></div>
        </div>
        <div className={`row ${style.listElementBody}`} style={bodyStyle}>
            {children[1]}
        </div>
    </div>
}

export const ListElement = ({children}: ListElementProps) => {
    return <div className={`container-fluid ${style.listElement}`}>
        <div className={`row ${style.listElementBody} pt-2 pb-3`}>
            {children}
        </div>
    </div>
}

export const CollapsedListElement = ({children}: CollapsedListElementProps) => {

    const [isCollapsed, setIsCollapsed] = React.useState(true)

    const headStyle = {}   

    const bodyStyle = {
        height : isCollapsed ? 0 : 'auto',
        paddingTop : isCollapsed ? 0 : 5,
    }

    return <div className={`container-fluid ${style.listElement}`}> 
        <div className={`row ${style.listElementHead}`} style={headStyle} onClick={() => setIsCollapsed(!isCollapsed)}>
            <div className='col-11'><div className='row'>{children[0]}</div></div>           
            <div className='col-1' style={{textAlign: 'right'}}><p className="fs-5">{isCollapsed ? <i className="bi bi-chevron-up"></i> : <i className="bi bi-chevron-down"></i>}</p></div>
        </div>
        <div className={`row ${style.listElementBody}`} style={bodyStyle}>
            {children[1]}
        </div>
    </div>
} 