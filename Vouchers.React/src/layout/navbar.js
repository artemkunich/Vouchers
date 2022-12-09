import React from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { Link } from 'react-router-dom'
import { logout } from '../api/userManager.ts'
import { noprofileImageSrc, getImageSrc } from '../imageSources.ts';

import "bootstrap-icons/font/bootstrap-icons.css";
import style from './navbar.module.css'

const Navbar = (props) => {

    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)
    const identityDetail = useSelector(state => state.user.identityDetail)
    const identityDetailImageSrc = identityDetail.croppedImageId ? getImageSrc(identityDetail.croppedImageId) : noprofileImageSrc

    const dispatch = useDispatch()

    console.log(identityDetailImageSrc)

    const setSideBarWidth = async () => {
        const sideBarWidth = props.sideBarWidth;
        const sideBarWidthSetter = props.sideBarWidthSetter;
        const sidebarMaxWidth = props.sidebarMaxWidth;

        if(sideBarWidth === 0)
            sideBarWidthSetter(sidebarMaxWidth)
        else
            sideBarWidthSetter(0)
    }

    return (
        <nav id="main-navbar" className="navbar navbar-dark bg-dark fixed-top" >
            <div className="container-fluid">
                <button type="button" className="navbar-toggler" data-bs-toggle="collapse" onClick={setSideBarWidth} aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>

                {/* <form className="d-flex">
                    <input className="form-control me-2" type="search" placeholder="Search" aria-label="Search"/>
                    <button className="btn btn-outline-success" type="submit">Search</button>
                </form> */}
                <div className={`${style.loginMenu} dropdown`}>
                    <a href="#" className="d-flex align-items-center text-white text-decoration-none " id="dropdownUser1" data-bs-toggle="dropdown" aria-expanded="false">
                        <img src={identityDetailImageSrc} alt="" width="36" height="36" className="rounded-circle me-1"/>
                    </a>
                    
                    <div className="dropdown-menu dropdown-menu-dark">
                        <p>{`${identityDetail.firstName} ${identityDetail.lastName}`}</p>
                        <p>{identityDetail.email}</p>    

                        <hr/>
                        <ul className="text-small shadow">                          
                            <li>
                                <Link to="/profile" className="dropdown-item"><i className="bi bi-card-text me-2"></i>Profile</Link>
                            </li>
                            <li>
                                <a className="dropdown-item" href="#"><i className="bi bi-card-text me-2"></i>Settings</a>
                            </li>
                            <hr className="dropdown-divider"/>
                            <li>
                                <a className="dropdown-item" href="#" onClick={logout}><i className="bi bi-card-text me-2" ></i>Sign out</a>
                            </li>
                        </ul>  
                    </div>              
                </div>
            </div>
        </nav>
    )
}

export default Navbar