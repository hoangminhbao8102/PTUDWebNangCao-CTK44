import React from "react";
import SearchForm from "./SearchForm";
import CategoriesWidget from "./CategoriesWidget";
import FeaturedPosts from "./Sidebar/FeaturedPosts";
import RandomPosts from "./Sidebar/RandomPosts";
import TagCloud from "./Sidebar/TagCloud";
import BestAuthors from "./Sidebar/BestAuthors";
import Archives from "./Sidebar/Archives";
import SubscribeForm from "./SubscribeForm";

const Sidebar = () => {
    return (
        <div className="pt-4 ps-2">
            <SearchForm />

            <CategoriesWidget />

            <FeaturedPosts />
            
            <RandomPosts />
            
            <TagCloud />
            
            <BestAuthors />
            
            <Archives />
            
            <SubscribeForm />
        </div>
    )
}

export default Sidebar;