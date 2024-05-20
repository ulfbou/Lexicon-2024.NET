var todoLists = {};
var itemList = {};

document.addEventListener('DOMContentLoaded', function() {

    function createTodoList(listName) {
        if (todoLists[listName] != null) {
            // TODO: Message the user that a list with that name already exists. 
            console.log("Attempting to clone a todo list.", listName);
            return;
        }

        console.log("About to create a new todo list: ", listName);
        
        // Create Wrapper element
        const todoListWrapper = document.createElement("div");
        todoListWrapper.classList.add("todo-list-wrapper");
        todoListWrapper.classList.add("col-md-4");

        // Create title
        const todoListTitle = document.createElement("h2");
        todoListTitle.textContent = listName;

        // Create ordered list
        const todoListItems = document.createElement("ol");
        todoListItems.classList.add("col-md-4");
        todoListItems.classList.add("todo-list-items");

        // Append title and list to wrapper
        todoListWrapper.appendChild(todoListTitle);
        todoListWrapper.appendChild(todoListItems);

        // Append wrapper to the list container
        const todoListsContainer = document.getElementById("todo-list-container");
        todoListsContainer.appendChild(todoListWrapper);

        // Create an option
        const option = document.createElement("option");
        option.textContent = listName;
        option.value = listName;

        // Append option to its selector
        // const todoListSelect = document.getElementById("todo-list-select");
        // todoListSelect.appendChild(option);

        todoLists[listName] = {
            element: todoListWrapper
        };
    }

    const todoListsContainer = document.getElementById("todo-list-container");
    createTodoList("Livsmedel");

    document.getElementById("new-todo-list-form").addEventListener("submit", function(event) {
        event.preventDefault();
        const newTodoListName = document.getElementById("new-todo-list-name").value;
        createTodoList(newTodoListName);
    });

    document.getElementById("add-todo-item-form").addEventListener("submit", function(event) {
        event.preventDefault();
    
        // const todoListSelect = document.getElementById("todo-list-select");
        // const selectedListName = todoListSelect.value;
    
        // if (itemList[selectedListName] != null) {
        //     console.log("Refusing to clone an item.");
        //     // TODO: Message the user that we cannot duplicate an item.
        //     return;
        // }
    
        // const selectedList = todoLists[selectedListName];
    
        const todoListItem = document.getElementById("todo-item-name");
        const todoItemName = todoListItem.value;
    
        // Retrieve the corresponding list wrapper from the todoLists object
        // const listWrapper = todoLists[selectedListName].element;
    
        // Create the <li> element
        const newTodoItem = document.createElement("li");
        newTodoItem.textContent = todoItemName;
    
        newTodoItem.addEventListener("click", function (event) {
            event.preventDefault();
    
            newTodoItem.classList.toggle('line-through');
        });
        
        newTodoItem.addEventListener("dblclick", function (event) {
            event.preventDefault();
        
            if (newTodoItem.classList.contains('line-through')) {
                const closestWrapper = newTodoItem.closest('.todo-list-wrapper');

                // Get the next sibling of the parent container
                if (closestWrapper.nextElementSibling != null) {
                    const nextList = newTodoItem.closest('.todo-list-wrapper').nextElementSibling.querySelector('ol');
        
                    if (nextList) {
                        nextList.appendChild(newTodoItem);
                        newTodoItem.classList.remove("line-through");
                    }
                     else {
                        console.log("There is no next list to move the item to.");
                    }
                }
                else {
                    console.log("There is no next list to move the item to.");
                }
            } 
            else {
                // Why toggle
                newTodoItem.classList.toggle('line-through');
            }
        });
          
        if (listWrapper == null) {
            console.log("ERROR!");
            console.log("ERROR!");
            return;
        }
        // Append the new <li> element to the corresponding list wrapper
        const ol = listWrapper.querySelector("ol");
        if (ol == null) {
            console.log("ERROR!");
            return;
        }
        ol.appendChild(newTodoItem);
    
        todoListItem.value = ""; // Clear the input field
    });

    // document.getElementById("todo-list-select").addEventListener("change", function(event) {
    //     const selectedList = event.target.value;
    //     // TODO: Implement todo list moving logic
    // });
});
