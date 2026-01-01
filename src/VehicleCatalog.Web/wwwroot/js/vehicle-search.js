(function() {
    'use strict';

    const makeSelect = document.getElementById('makeSelect');
    const makeSearch = document.getElementById('makeSearch');
    const makeDropdown = document.getElementById('makeDropdown');
    const yearInput = document.getElementById('yearInput');
    const searchBtn = document.getElementById('searchBtn');
    const loadingIndicator = document.getElementById('loadingIndicator');
    const resultsContainer = document.getElementById('resultsContainer');
    const vehicleTypesSection = document.getElementById('vehicleTypesSection');
    const vehicleModelsSection = document.getElementById('vehicleModelsSection');
    const vehicleTypesList = document.getElementById('vehicleTypesList');
    const vehicleModelsList = document.getElementById('vehicleModelsList');
    const errorContainer = document.getElementById('errorContainer');
    const errorMessage = document.getElementById('errorMessage');
    const noResultsContainer = document.getElementById('noResultsContainer');

    let currentMakeId = null;
    let currentYear = null;
    let highlightedIndex = -1;

    const dropdownItems = makeDropdown.querySelectorAll('.dropdown-item');
    
    makeSearch.addEventListener('focus', function() {
        showDropdown();
        filterDropdown('');
    });

    makeSearch.addEventListener('input', function() {
        const searchTerm = this.value.toLowerCase().trim();
        filterDropdown(searchTerm);
        showDropdown();
    });

    makeSearch.addEventListener('keydown', function(e) {
        const visibleItems = Array.from(dropdownItems).filter(item => !item.classList.contains('hidden'));
        
        switch(e.key) {
            case 'ArrowDown':
                e.preventDefault();
                highlightedIndex = Math.min(highlightedIndex + 1, visibleItems.length - 1);
                updateHighlight(visibleItems);
                break;
            case 'ArrowUp':
                e.preventDefault();
                highlightedIndex = Math.max(highlightedIndex - 1, 0);
                updateHighlight(visibleItems);
                break;
            case 'Enter':
                e.preventDefault();
                if (highlightedIndex >= 0 && visibleItems[highlightedIndex]) {
                    selectItem(visibleItems[highlightedIndex]);
                }
                break;
            case 'Escape':
                hideDropdown();
                break;
            case 'Tab':
                hideDropdown();
                break;
        }
    });

    dropdownItems.forEach(item => {
        item.addEventListener('click', function() {
            selectItem(this);
        });

        item.addEventListener('mouseenter', function() {
            dropdownItems.forEach(i => i.classList.remove('highlighted'));
            this.classList.add('highlighted');
        });
    });

    function filterDropdown(searchTerm) {
        let hasVisibleItems = false;
        highlightedIndex = -1;
        
        dropdownItems.forEach(item => {
            const text = item.getAttribute('data-text').toLowerCase();
            if (text.includes(searchTerm)) {
                item.classList.remove('hidden');
                hasVisibleItems = true;
            } else {
                item.classList.add('hidden');
            }
            item.classList.remove('highlighted');
        });

        let noResultsMsg = makeDropdown.querySelector('.no-results-dropdown');
        if (!hasVisibleItems) {
            if (!noResultsMsg) {
                noResultsMsg = document.createElement('div');
                noResultsMsg.className = 'no-results-dropdown';
                noResultsMsg.textContent = 'No makes found matching your search';
                makeDropdown.appendChild(noResultsMsg);
            }
            noResultsMsg.style.display = 'block';
        } else if (noResultsMsg) {
            noResultsMsg.style.display = 'none';
        }
    }

    function updateHighlight(visibleItems) {
        dropdownItems.forEach(item => item.classList.remove('highlighted'));
        if (visibleItems[highlightedIndex]) {
            visibleItems[highlightedIndex].classList.add('highlighted');
            visibleItems[highlightedIndex].scrollIntoView({ block: 'nearest' });
        }
    }

    function selectItem(item) {
        const value = item.getAttribute('data-value');
        const text = item.getAttribute('data-text');
        
        makeSelect.value = value;
        makeSearch.value = text;
        
        dropdownItems.forEach(i => i.classList.remove('selected'));
        item.classList.add('selected');
        
        hideDropdown();
        yearInput.focus();
    }

    function showDropdown() {
        makeDropdown.classList.add('show');
    }

    function hideDropdown() {
        makeDropdown.classList.remove('show');
        highlightedIndex = -1;
    }

    document.addEventListener('click', function(e) {
        if (!e.target.closest('.searchable-select')) {
            hideDropdown();
        }
    });

    searchBtn.addEventListener('click', handleSearch);
    yearInput.addEventListener('keypress', function(e) {
        if (e.key === 'Enter') {
            handleSearch();
        }
    });

    async function handleSearch() {
        currentMakeId = parseInt(makeSelect.value);
        currentYear = parseInt(yearInput.value);

       if (!currentMakeId) {
            showError('Please select a vehicle make');
            return;
        }

        if (!currentYear || currentYear < 1900 || currentYear > new Date().getFullYear() + 2) {
            showError('Please enter a valid year between 1900 and ' + (new Date().getFullYear() + 2));
            return;
        }

        hideAllSections();
        showLoading();

        try {
            const types = await fetchVehicleTypes(currentMakeId);
           
            const models = await fetchModels(currentMakeId, currentYear);

            hideLoading();
            
            if (types && types.length > 0) {
                displayVehicleTypes(types);
            }

            if (models && models.length > 0) {
                displayModels(models);
            }

            if ((!types || types.length === 0) && (!models || models.length === 0)) {
                showNoResults();
            } else {
                resultsContainer.style.display = 'block';
                resultsContainer.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
            }

        } catch (error) {
            hideLoading();
            showError('Failed to retrieve vehicle data. Please try again.');
            console.error('Search error:', error);
        }
    }

    async function fetchVehicleTypes(makeId) {
        const response = await fetch(`/Home/GetVehicleTypes?makeId=${makeId}`);
        if (!response.ok) {
            throw new Error('Failed to fetch vehicle types');
        }
        return await response.json();
    }

    async function fetchModels(makeId, year) {
        const response = await fetch(`/Home/GetModels?makeId=${makeId}&year=${year}`);
        if (!response.ok) {
            throw new Error('Failed to fetch models');
        }
        return await response.json();
    }

    function displayVehicleTypes(types) {
        vehicleTypesList.innerHTML = '';
        
        types.forEach((type, index) => {
            const card = document.createElement('div');
            card.className = 'type-card';
            card.style.animationDelay = `${index * 0.05}s`;
            card.innerHTML = `
                <div class="type-icon">🏷️</div>
                <div class="type-name">${escapeHtml(type.vehicleTypeName)}</div>
            `;
            vehicleTypesList.appendChild(card);
        });

        vehicleTypesSection.style.display = 'block';
    }

    function displayModels(models) {
        vehicleModelsList.innerHTML = '';
        
        models.forEach((model, index) => {
            const card = document.createElement('div');
            card.className = 'model-card';
            card.style.animationDelay = `${index * 0.03}s`;
            card.innerHTML = `
                <div class="model-header">
                    <div class="model-icon">🚙</div>
                    <div class="model-year">${model.modelYear}</div>
                </div>
                <div class="model-name">${escapeHtml(model.modelName)}</div>
            `;
            vehicleModelsList.appendChild(card);
        });

        vehicleModelsSection.style.display = 'block';
    }

    function showLoading() {
        loadingIndicator.style.display = 'flex';
    }

    function hideLoading() {
        loadingIndicator.style.display = 'none';
    }

    function showError(message) {
        errorMessage.textContent = message;
        errorContainer.style.display = 'block';
        setTimeout(() => {
            errorContainer.style.display = 'none';
        }, 5000);
    }

    function showNoResults() {
        noResultsContainer.style.display = 'block';
    }

    function hideAllSections() {
        resultsContainer.style.display = 'none';
        vehicleTypesSection.style.display = 'none';
        vehicleModelsSection.style.display = 'none';
        errorContainer.style.display = 'none';
        noResultsContainer.style.display = 'none';
    }

    function escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }

})();
